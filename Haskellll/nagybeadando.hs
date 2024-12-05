module Beadando where

type Coordinate = (Int, Int)
type Sun = Int

data Plant = Peashooter Int | Sunflower Int | Walnut Int | CherryBomb Int
    deriving (Show, Eq)

data Zombie = Basic Int Int | Conehead Int Int | Buckethead Int Int | Vaulting Int Int
    deriving (Show, Eq)

data GameModel = GameModel Sun [(Coordinate, Plant)] [(Coordinate, Zombie)]
    deriving (Show, Eq)

defaultPeashooter :: Plant
defaultPeashooter = Peashooter 3

defaultSunflower :: Plant
defaultSunflower = Sunflower 2

defaultWalnut :: Plant
defaultWalnut = Walnut 15

defaultCherryBomb :: Plant
defaultCherryBomb = CherryBomb 2

basic :: Zombie
basic = Basic 5 1

coneHead :: Zombie
coneHead = Conehead 10 1

bucketHead :: Zombie
bucketHead = Buckethead 20 1

vaulting :: Zombie
vaulting = Vaulting 7 2







tryPurchase :: GameModel -> Coordinate -> Plant -> Maybe GameModel
tryPurchase (GameModel nap novenyek zombik ) mkoord@(x1, x2) tipus
    | x1 > 4 || x1 < 0 || x2 > 11 || x2 < 0 = Nothing
    | tipus == defaultPeashooter && nap < 100 = Nothing
    | (tipus == defaultSunflower || tipus == defaultWalnut ) && nap < 50 = Nothing
    | tipus == defaultCherryBomb && nap < 150 = Nothing
    | koordCheck novenyek mkoord == True = Nothing
    | otherwise = (Just (GameModel (subtractSun nap tipus) (novenyek ++ [(mkoord, tipus)]) zombik))

koordCheck :: [(Coordinate, Plant)] -> Coordinate -> Bool
koordCheck [] _ = False
koordCheck ((koord, _):xs) mkoord
    | koord == mkoord = True
    | otherwise = False || koordCheck xs mkoord

subtractSun :: Sun -> Plant -> Sun
subtractSun nap tipus
    | tipus == defaultPeashooter = (nap - 100)
    | tipus == defaultCherryBomb = (nap - 150)
    | otherwise = (nap - 50)









zombieCheck :: [(Coordinate, Zombie)] -> Int -> Bool
zombieCheck [] _ = False
zombieCheck (((x, y), _):xs) lane
    | x == lane && y == 11 = True
    | otherwise = False || zombieCheck xs lane

placeZombieInLane :: GameModel -> Zombie -> Int -> Maybe GameModel
placeZombieInLane (GameModel nap novenyek zombik ) tipus lane
    | lane < 0 || lane > 4 = Nothing
    | zombieCheck zombik lane == True = Nothing
    | otherwise = (Just (GameModel nap novenyek ([((lane, 11), tipus)] ++ zombik )))





checkIfPlantAlive :: Coordinate -> [(Coordinate, Plant)] -> Bool
checkIfPlantAlive zkoord ((nkoord, ntipus):xs)
    | zkoord == nkoord && halottN ntipus == True = True
    | zkoord == nkoord && halottN ntipus == False = False

isThereAPlant :: Coordinate -> [(Coordinate, Plant)] -> Bool
isThereAPlant _ [] = False
isThereAPlant (z1, z2) (((p1, p2), _):xs)
    | z1 == p1 && z2 == p2 = True
    | otherwise = isThereAPlant (z1, z2) xs


isThereAZombie :: Coordinate -> [(Coordinate, Zombie)] -> Bool
isThereAZombie _ [] = False
isThereAZombie (p1, p2) (((z1, z2), _):xs)
    | p1 == z1 && p2 == z2 = True
    | otherwise = isThereAZombie (p1, p2) xs

zombieSelector :: Zombie -> Int
zombieSelector (Vaulting hp 2) = 0
zombieSelector _ = 1

move :: [(Coordinate, Plant)] -> [(Coordinate,Zombie)] -> [(Coordinate, Zombie)]
move _ [] = []
move novenyek (x@((z1, z2), ztipus):xs)
    | isThereAPlant (z1, z2) novenyek == False && isThereAPlant (z1, (z2-1)) novenyek == False && zombieSelector ztipus == 0 = [(moveFullVaulting x)] ++ (move novenyek xs)
    | isThereAPlant (z1, z2) novenyek == False && isThereAPlant (z1, (z2-1)) novenyek == True && zombieSelector ztipus == 0 = [ changeVault (moveFullVaulting x) ] ++ (move novenyek xs)
    | isThereAPlant (z1, z2) novenyek == True && zombieSelector ztipus == 0 = [moveSimple(changeVault x)] ++ (move novenyek xs)
    | isThereAPlant (z1, z2) novenyek == False && zombieSelector ztipus == 1 = [moveSimple x] ++ (move novenyek xs)
    | isThereAPlant (z1, z2) novenyek == True && zombieSelector ztipus == 1 = [x] ++ (move novenyek xs)

moveFullVaulting :: (Coordinate, Zombie) ->(Coordinate, Zombie)
moveFullVaulting ((z1, z2), ztipus) = ((z1, z2-2), ztipus)

changeVault :: (Coordinate, Zombie) -> (Coordinate, Zombie)
changeVault (zkoord, (Vaulting hp 2)) = (zkoord, (Vaulting hp 1))

moveSimple :: (Coordinate, Zombie) -> (Coordinate, Zombie)
moveSimple ((z1, z2), ztipus) = ((z1, z2-1), ztipus)

--moveBigger :: GameModel -> GameModel
--moveBigger (GameModel nap novenyek zombik) = (GameModel nap novenyek (move novenyek zombik))

endGame :: [(Coordinate, Zombie)] -> Bool
endGame [] = False
endGame (((_, z2), _):xs)
    | z2 <= 0 = True
    | otherwise = endGame xs

performZombieActions :: GameModel -> Maybe GameModel
performZombieActions (GameModel nap novenyek zombik)
    | endGame zombik == True = Nothing
    | otherwise = (Just (GameModel nap (bite novenyek zombik) (move novenyek zombik)))

isThereAZombie' :: Coordinate -> [(Coordinate, Zombie)] -> Bool
isThereAZombie' _ [] = False
isThereAZombie' (p1, p2) (((z1, z2), ztipus):xs) -- ztipus (Vaulting hp speed)
    | p1 == z1 && p2 == z2 && zombieSelector ztipus == 1 = True
    | otherwise = isThereAZombie' (p1, p2) xs

bite :: [(Coordinate, Plant)] -> [(Coordinate, Zombie)] -> [(Coordinate, Plant)]
bite [] _ = []
bite (x@(nkoord, ntipus):xs) zombik
    | isThereAZombie' nkoord zombik == True = [(biteSmall x zombik)] ++ (bite xs zombik)
    | otherwise = [x] ++ (bite xs zombik)

biteSmall :: (Coordinate, Plant) -> [(Coordinate, Zombie)] -> (Coordinate, Plant)
biteSmall n [] = n
biteSmall noveny@(nkoord, ntipus) ((zkoord, ztipus):xs)
    | nkoord == zkoord && zombieSelector ztipus == 1 = biteSmall (damagePlant noveny) xs
    | otherwise = biteSmall noveny xs

damagePlant :: (Coordinate, Plant) -> (Coordinate, Plant)
damagePlant (nkoord, (Peashooter 0))    = (nkoord, (Peashooter 0))
damagePlant (nkoord, (Walnut 0))        = (nkoord, (Walnut 0))
damagePlant (nkoord, (CherryBomb 0))    = (nkoord, (CherryBomb 0))
damagePlant (nkoord, (Sunflower 0))     = (nkoord, (Sunflower 0))
damagePlant (nkoord, (Peashooter hp))   = (nkoord, (Peashooter (hp-1)))
damagePlant (nkoord, (Walnut hp))       = (nkoord, (Walnut (hp-1)))
damagePlant (nkoord, (CherryBomb hp))   = (nkoord, (CherryBomb (hp-1)))
damagePlant (nkoord, (Sunflower hp))    = (nkoord, (Sunflower (hp-1)))










cleanBoard :: GameModel -> GameModel
cleanBoard (GameModel nap novenyek zombik) = (GameModel nap (clearNoveny novenyek) (clearZombik zombik))

clearNoveny :: [(Coordinate, Plant)] -> [(Coordinate, Plant)]
clearNoveny [] = []
clearNoveny (x@(nkoord, ntipus):xs)
    | halottN ntipus == True = clearNoveny xs
    | otherwise = [x] ++ clearNoveny xs

halottN :: Plant -> Bool
halottN (Peashooter n)
    | n <= 0 = True
    | otherwise = False
halottN (CherryBomb n)
    | n <= 0 = True
    | otherwise = False
halottN (Walnut n)
    | n <= 0 = True
    | otherwise = False
halottN (Sunflower n)
    | n <= 0 = True
    | otherwise = False

clearZombik :: [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
clearZombik [] = []
clearZombik (x@(nkoord, ntipus):xs)
    | halottZ ntipus == True = clearZombik xs
    | otherwise = [x] ++ clearZombik xs

halottZ :: Zombie -> Bool
halottZ (Basic n _)
    | n <= 0 = True
    | otherwise = False
halottZ (Conehead n _)
    | n <= 0 = True
    | otherwise = False
halottZ (Buckethead n _)
    | n <= 0 = True
    | otherwise = False
halottZ (Vaulting n _)
    | n <= 0 = True
    | otherwise = False








--Extra

plantCheck :: Plant -> Int
plantCheck (Peashooter _) = 1
plantCheck (Sunflower _) = 2
plantCheck (CherryBomb _) = 3
plantCheck _ = 0

damage :: Zombie -> Zombie
damage (Basic 0 speed) = (Basic 0 speed)
damage (Conehead 0 speed) = (Conehead 0 speed)
damage (Buckethead 0 speed) = (Buckethead 0 speed)
damage (Vaulting 0 speed) = (Vaulting 0 speed)
damage (Basic hp speed) = (Basic (hp-1) speed)
damage (Conehead hp speed) = (Conehead (hp-1) speed)
damage (Buckethead hp speed) = (Buckethead (hp-1) speed)
damage (Vaulting hp speed) = (Vaulting (hp-1) speed)

shootBigger :: Coordinate -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
shootBigger (_, 12) zombik = zombik
shootBigger (p1, p2) zombik
    | isThereAZombie (p1, p2) zombik == True = shoot (p1, p2) zombik
    | otherwise = shootBigger (p1, (p2+1)) zombik

shoot :: Coordinate -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
shoot _ [] = []
shoot (p1, p2) (x@((z1, z2), ztipus):xs)
    | p1 == z1 && p2 == z2 = [((z1, z2), (damage ztipus))] ++ (shoot (p1, p2) xs)
    | otherwise = [x] ++ (shoot (p1, p2) xs)

generateSun :: Sun -> [(Coordinate, Plant)] -> Sun
generateSun nap [] = nap
generateSun nap ((nkoord, ntipus):xs)
    | plantCheck ntipus == 2 = 25 + (generateSun nap xs)
    | otherwise = (generateSun nap xs)

decimate :: Zombie -> Zombie
decimate (Basic _ speed) = (Basic 0 speed)
decimate (Conehead _ speed) = (Conehead 0 speed)
decimate (Buckethead _ speed) = (Buckethead 0 speed)
decimate (Vaulting _ speed) = (Vaulting 0 speed)

absolutelyDecimate :: Coordinate -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
absolutelyDecimate _ [] = []
absolutelyDecimate nkoord (((z1, z2), ztipus):xs)
    | ((z1 - 1),(z2 - 1))   == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1),(z2 - 1))       == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1 + 1),(z2 - 1))   == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1 - 1),(z2))       == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | (z1, z2)              == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1 + 1),(z2))       == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1 - 1),(z2 + 1))   == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1),(z2 + 1))       == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | ((z1 + 1),(z2 + 1))   == nkoord   = [((z1, z2), (decimate ztipus))] ++ (absolutelyDecimate (nkoord) (xs))
    | otherwise                         = [((z1, z2), ztipus)]            ++ (absolutelyDecimate (nkoord) (xs))

attack :: [(Coordinate, Plant)] -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
attack [] zombik = zombik
attack ((nkoord, ntipus):xs) zombik
    | plantCheck ntipus == 1 = (attack xs (shootBigger nkoord zombik))
    | plantCheck ntipus == 3 = (attack xs (absolutelyDecimate nkoord zombik))
    | otherwise = (attack xs zombik)

explode :: [(Coordinate, Plant)] -> [(Coordinate, Plant)]
explode [] = []
explode (x@(nkoord, ntipus):xs)
    | plantCheck ntipus == 3 = [(nkoord, (CherryBomb 0))] ++ explode xs
    | otherwise = [x] ++ explode xs

performPlantActions :: GameModel -> GameModel
performPlantActions (GameModel nap novenyek zombik) = (GameModel (generateSun nap novenyek) (explode novenyek) (attack novenyek zombik))








getSun :: GameModel -> GameModel
getSun (GameModel nap n z) = (GameModel (nap + 25) n z)

--loseRoyal :: GameModel
--loseRoyal = (GameModel 9999 [((-10,-10), defaultSunflower)] [])

reinforce :: GameModel -> [(Int, Zombie)] -> GameModel
reinforce modell [] = modell
reinforce modell@(GameModel nap novenyek zombik) (x@(lane, ztipus):xs)
    | placeZombieInLane modell ztipus lane == Nothing = error "nem lehet beszurni a zombot"
    | otherwise = reinforce (deMaybeing (placeZombieInLane modell ztipus lane)) xs

deMaybeing :: Maybe GameModel -> GameModel
deMaybeing (Just modell) = modell

defendsAgainst :: GameModel -> [[(Int, Zombie)]] -> Bool
defendsAgainst modell (x:xs)
    | performZombieActions (cleanBoard (performPlantActions modell)) == Nothing = False
    | otherwise = defendsAgainst (getSun (cleanBoard (reinforce (deMaybeing (performZombieActions (cleanBoard (performPlantActions modell)))) x))) xs
defendsAgainst modell@(GameModel nap novenyek zombik) []
    | zombik == [] = True
    | performZombieActions (cleanBoard (performPlantActions modell)) == Nothing = False
    | otherwise = defendsAgainst (getSun (cleanBoard (reinforce (deMaybeing (performZombieActions (cleanBoard (performPlantActions modell)))) []))) []

defendsAgainstI :: (GameModel -> GameModel) -> GameModel -> [[(Int, Zombie)]] -> Bool
defendsAgainstI fgv modell@(GameModel nap novenyek zombik) (x:xs)
    | performZombieActions' (cleanBoard (performPlantActions (fgv modell))) == Nothing = False
    | otherwise = defendsAgainstI fgv (getSun (cleanBoard (reinforce (deMaybeing (performZombieActions' (cleanBoard (performPlantActions (fgv modell))))) x))) xs
defendsAgainstI fgv modell@(GameModel nap novenyek zombik) []
    | zombik == [] = True
    | performZombieActions' (cleanBoard (performPlantActions (fgv modell))) == Nothing = False
    | otherwise = defendsAgainstI fgv (getSun (cleanBoard (reinforce (deMaybeing (performZombieActions' (cleanBoard (performPlantActions (fgv modell))))) []))) []

performZombieActions' :: GameModel -> Maybe GameModel
performZombieActions' (GameModel nap novenyek zombik)
    | endGame (move novenyek zombik) == True = Nothing
    | otherwise = (Just (GameModel nap (bite novenyek zombik) (move novenyek zombik)))