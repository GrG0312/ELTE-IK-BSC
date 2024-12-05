<?php
session_start();
?>

Authenticated as:
<?= $_SESSION['user']['username'] ?>

<?php
include 'util.php';
?>

<?php if(in_array('admin', $_SESSION['user']['roles'])): ?>
  <button>Admin</button>
<?php endif; ?>

<form action="" method="post">
    <input name="logout" hidden />
    <button type="submit">Logout</button>
</form>
<?php
if (!empty($_POST) && isset($_POST['logout'])) {
    unset($_SESSION['user']);
    redirect('login.php');
}