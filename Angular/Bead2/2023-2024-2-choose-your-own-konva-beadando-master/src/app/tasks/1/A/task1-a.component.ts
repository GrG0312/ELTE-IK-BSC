import {AfterViewInit, Component, ElementRef, OnDestroy} from '@angular/core';
import Konva from "konva";
import { shapes } from 'konva/lib/Shape';
import { Arc } from 'konva/lib/shapes/Arc';
import { Vector2d } from 'konva/lib/types';
import { interval } from 'rxjs';

@Component({
  selector: 'app-task1-a',
  templateUrl: './task1-a.component.html',
  styleUrls: ['./task1-a.component.less']
})
export class Task1AComponent implements AfterViewInit, OnDestroy {
  selectedLayer?: Konva.Layer;
  stage?: Konva.Stage;
  angle = 270;
  shapeType = "arc";

  canStart : boolean = true;
  animation? : Konva.Animation;
  interval : any;
  timeLeft : number = 5;
  backgroundLayer? : Konva.Layer;

  pointGroup? : Konva.Group;
  edgeGroup? : Konva.Group;
  constructor(private el: ElementRef) { }

  ngAfterViewInit() {
    setTimeout(() => { // Forcing a single change detection cycle delay
      this.stage = new Konva.Stage({
        container: 'konva-container',
        width: this.el.nativeElement.offsetWidth,
        height: 500,
      });
      const layer = new Konva.Layer();
      this.backgroundLayer = new Konva.Layer({
        listening: false
      });
      this.stage.add(layer);
      this.stage.add(this.backgroundLayer);

      this.selectedLayer = this.stage.getLayers()[0];

      this.edgeGroup = new Konva.Group();
      this.selectedLayer.add(this.edgeGroup);
      //Click event handler
      this.stage.on('click', (event) => {
        this.ClickHandlerMethod(event);
      });
    });

  }

  UpdateAngles(){
    //console.log("Method runs");
    this.selectedLayer?.getChildren().forEach(shape => {
      if(shape instanceof Konva.Arc){
        //console.log("Trying to update the Shape");
        shape.angle(this.angle);
      }
    });
    //??? FOR REAL THO??
    //does not update automatically
    this.selectedLayer?.draw();
  }

  NewEdgeArc(ax : number, ay : number){
    if(this.pointGroup){
      var newLine : Konva.Line = new Konva.Line({
        points: [ax, ay, this.pointGroup.x(), this.pointGroup.y()],
        stroke: 'blue',
        strokeWidth: 4
      });
      this.edgeGroup?.add(newLine);
      this.UpdateEdgeNumberDisplay();
    }
  }

  UpdateEdges(px : number, py : number){
    this.edgeGroup?.getChildren().forEach(child => {
      if(child instanceof Konva.Line){
        var ax : number = child.points()[0];
        var ay : number = child.points()[1];
        child.points([ax,ay, px,py]);
      }
    });
    this.edgeGroup?.draw();
  }

  NewEdgePoint(){
    this.selectedLayer?.getChildren().forEach(child => {
      if(child instanceof Konva.Arc){
        var newLine : Konva.Line = new Konva.Line({
          points: [child.x(), child.y(), this.pointGroup!.x(), this.pointGroup!.y()],
          stroke: 'blue',
          strokeWidth: 4
        });
        this.edgeGroup?.add(newLine);
      }
    });
    this.UpdateEdgeNumberDisplay();
  }

  UpdateEdgeNumberDisplay(){
    if(this.pointGroup){
      var label : Konva.Label = (this.pointGroup.getChildren()[1]) as Konva.Label;
      //console.log(label);
      var text : Konva.Text = (label.getChildren()[1]) as Konva.Text;
      //console.log(text);
      //console.log(this.edgeGroup?.getChildren().length);
      text.text(`Number of edges: ${this.edgeGroup?.getChildren().length}`);
    }
  }

  PointRandomMove(){
    var targetX : number = Math.random() * this.selectedLayer!.width();
    targetX = Math.round(targetX);
    var targetY : number = Math.random() * this.selectedLayer!.height();
    targetY = Math.round(targetY);
    //console.log(`Target of Point: ${targetX}, ${targetY}`);
    //console.log(`Current loc of Point: ${this.pointGroup?.x()}, ${this.pointGroup?.y()}`);
    
    //this would be the instant travel solution
    //this.pointGroup?.position({x: targetX, y: targetY});
    //this.UpdateEdges(targetX, targetY);

    //animated solution
    var distanceUnitX : number = (targetX - this.pointGroup!.x()) / 1000;
    var distanceUnitY : number = (targetY - this.pointGroup!.y()) / 1000;
    //console.log(`Distance unit: ${distanceUnitX}, ${distanceUnitY}`);

    this.animation = new Konva.Animation((frame) => {
      if(Math.abs(targetX - this.pointGroup!.x()) <= Math.abs(distanceUnitX * frame!.timeDiff) && Math.abs(targetY - this.pointGroup!.y()) <= Math.abs(distanceUnitY * frame!.timeDiff)){
        this.pointGroup?.position({x: targetX, y:targetY});
        this.animation!.stop();
      } else {
        var currentX : number = this.pointGroup!.x() + distanceUnitX * frame!.timeDiff;
        var currentY : number = this.pointGroup!.y() + distanceUnitY * frame!.timeDiff;

        this.pointGroup?.position({x: currentX, y: currentY});
        this.UpdateEdges(currentX, currentY);
        this.ConditionalMinSearch();
      }
    }, this.selectedLayer);
    if(this.canStart){
      this.animation.start();
    }
  }

  VectorDistanceToPoint(ax : number, ay : number) : number{
    var x : number = this.pointGroup!.x() - ax;
    var y : number = this.pointGroup!.y() - ay;
    return Math.sqrt(Math.pow(x, 2) + Math.pow(y, 2));
  }

  ConditionalMinSearch(){
    var min : number;
    var found : boolean = false;
    var target : any;
    if(this.pointGroup){
      this.selectedLayer?.getChildren().forEach(child => {
        if(child instanceof Konva.Arc){
          child.fill('aqua');
          if(found && this.pointGroup){
            var temp : number = this.VectorDistanceToPoint(child.x(), child.y());
            if(min > temp){
              min = temp;
              target = child;
            }
          } else {
            found = true;
            min = this.VectorDistanceToPoint(child.x(), child.y());
            target = child;
          }
        }
      });
    }
    if(found){
      target = target as Konva.Arc;
      target.fill('red');
    }
  }

  OnDragOver(event : DragEvent){
    event.preventDefault();
    event.stopPropagation();
  }

  OnDrop(event : DragEvent){
    event.preventDefault();
    event.stopPropagation();
    console.log("Something just droppped!");

    var files = event.dataTransfer?.files;
    if(files && files.length > 0){
      var backgroundImage = files[0];
      var typeOfImage = backgroundImage.type;
      var validTypes = ['image/jpeg', 'image/bmp', 'image/svg+xml'];
      if(validTypes.includes(typeOfImage)){
        console.log(`This is a valid Image Type! ${backgroundImage.webkitRelativePath}`);
        this.SetBackGroundImage(backgroundImage);
      }
    }
    this.backgroundLayer?.on('click', (event) => {
      this.ClickHandlerMethod(event);
    });
  }

  ClickHandlerMethod(event : any){
    if (this.stage) {
      let pointer = this.stage.getPointerPosition();
      
      //If the pointer exists and we hit the canvas
      console.log(event.target instanceof Konva.Stage);
      if (pointer && event.target instanceof Konva.Stage) {
        switch (this.shapeType){
          case "arc":
            //We create the shape we need to place
            //console.log("Placing shape");
            var arcVar = new Konva.Arc({
              x: pointer.x,
              y: pointer.y,
              innerRadius: 40,
              outerRadius: 70,
              angle: this.angle,
              fill: 'aqua',
              stroke: 'blue',
              strokeWidth: 4
            });
            //console.log("Shape added");
            //We place the shape?
            this.selectedLayer?.add(arcVar);
            //We create a new edge for this, if the pointVar doesnt exist yet, this does nothing
            this.NewEdgeArc(arcVar.x(), arcVar.y());
            this.ConditionalMinSearch();
            break;
          case "point":
            //if the pointVar already exists
            if(this.pointGroup){
              //modify the x and y values
              this.pointGroup.position({x : pointer.x, y : pointer.y});
              //learning from mistake, I have to redraw it
              this.pointGroup.draw();
              //I have to update every existing line to match the new coords
              this.UpdateEdges(this.pointGroup.x(), this.pointGroup.y());
              this.ConditionalMinSearch();
            } else {
              //else I have to create the variable the same way I created the arc
              this.pointGroup = new Konva.Group({
                x: pointer.x,
                y: pointer.y,
                draggable: true
              });
              var pointVar = new Konva.Circle({
                x: 0,
                y: 0,
                radius: 35,
                fill: 'aqua',
                stroke: 'blue',
                strokeWidth: 4
              });
              var pointLabel : Konva.Label = new Konva.Label({
                x: -150,
                y: -75,
                opacity: 0.75
              });
              pointLabel.add(
                new Konva.Tag({
                  fill: 'black',
                  shadowColor: 'black',
                  shadowBlur: 10,
                  shadowOffsetX: 10,
                  shadowOffsetY: 10,
                  shadowOpacity: 0.5,
                })
              );
              pointLabel.add(
                new Konva.Text({
                  text: `Number of edges: 0`,
                  fill: 'white',
                  fontFamily: 'Calibri',
                  fontSize: 18,
                  padding: 5
                })
              );
              this.pointGroup.add(pointVar)
              this.pointGroup.add(pointLabel);
              this.pointGroup.on('dragstart', () => {
                this.canStart = false;
                this.animation?.stop();
              });
              this.pointGroup.on('dragend', () => {
                this.canStart = true;
              });
              this.pointGroup.on('dragmove', () => {
                this.UpdateEdges(this.pointGroup!.x(), this.pointGroup!.y());
                this.ConditionalMinSearch();
              });
              this.selectedLayer?.add(this.pointGroup);
              //and I have to create edges for every existing arc
              this.NewEdgePoint();
              this.ConditionalMinSearch();
              this.interval = setInterval(() => {
                if(this.timeLeft > 0){
                  this.timeLeft--;
                } else{
                  this.timeLeft = 5;
                  this.PointRandomMove();
                }
              }, 1000);
            }
            this.animation?.stop();
            this.timeLeft = 5;
            break;
          default:
            console.log("Unexpected behaviour");
            break;
        }
      }
    }
  }

  SetBackGroundImage(bg : File){
    var reader = new FileReader();
    reader.onload = (e) => {
      var img = new Image();
      img.onload = () => {
        var kImage = new Konva.Image({
          x: 0,
          y: 0,
          image: img,
          width: this.stage?.width(),
          height: this.stage?.height(),
          draggable: false
        });
        this.backgroundLayer?.destroyChildren();
        this.backgroundLayer?.add(kImage);
        this.backgroundLayer?.moveToBottom();
        
      };
      img.src = e.target?.result as string;
    };
    reader.readAsDataURL(bg);
  }

  ngOnDestroy(): void {
    clearInterval(this.interval);
    this.animation?.stop();
  }
}
