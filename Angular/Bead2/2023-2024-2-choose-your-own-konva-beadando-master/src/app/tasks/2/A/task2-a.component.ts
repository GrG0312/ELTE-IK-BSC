import {AfterViewInit, Component, ElementRef, Renderer2, input, numberAttribute} from '@angular/core';
import Konva from "konva";
import {Task2AKonvaMode} from "./models/task-2a-mode.model";
import {Tree} from "../../../_shapes/tree";
import { Layer } from 'konva/lib/Layer';

@Component({
  selector: 'app-task2-a',
  templateUrl: './task2-a.component.html',
  styleUrls: ['./task2-a.component.less']
})
export class Task2AComponent implements AfterViewInit {
  selectedMode: Task2AKonvaMode = Task2AKonvaMode.SELECT;
  selectedLayer?: Konva.Layer;
  stage?: Konva.Stage;
  transformer?: Konva.Transformer;

  layerArray : Konva.Layer[] = [];
  isEditMode : boolean = false;

  Task2AKonvaMode = Task2AKonvaMode;
  constructor(private el: ElementRef) { }

  ngAfterViewInit() {
    setTimeout(() => { // Forcing a single change detection cycle delay
      this.stage = new Konva.Stage({
        container: 'konva-container',
        width: this.el.nativeElement.offsetWidth,
        height: 500,
      });

      this.CreateNewLayer("Default 1");
      this.CreateNewLayer("Default 2");

      this.selectedLayer = this.stage.getLayers()[0];

      this.transformer = new Konva.Transformer();
      this.selectedLayer.add(this.transformer);

      this.stage.on('click', (event) => {
        if (this.stage) {
          let pointer = this.stage.getPointerPosition();
          if (pointer && event.target instanceof Konva.Stage) {
            switch (this.selectedMode) {
              case Task2AKonvaMode.RECTANGLE:
                const rectangle = new Konva.Rect({
                  x: pointer?.x,
                  y: pointer?.y,
                  width: 100,
                  height: 30,
                  stroke: 'black',
                  strokeWidth: 3,
                  draggable: true,
                });
                this.selectedLayer?.add(rectangle);
                break;
              case Task2AKonvaMode.SELECT:
                this.transformer?.nodes([]);
                break;
              case Task2AKonvaMode.TREE:
                const tree = new Tree(
                  pointer?.x,
                  pointer?.y,
                  50,
                  75,
                  true
                );
                this.selectedLayer?.add(tree.shape());
                break;
            }
          } else {
            switch (this.selectedMode) {
              case Task2AKonvaMode.SELECT:
                this.transformer?.nodes([event.target]);
                break;
            }
          }

        }
      });

    });

  }

  CreateNewLayer(pname? : string){
    const newLayer : Konva.Layer = new Konva.Layer();
    if(pname){
      newLayer.name(pname);
    } else {
      newLayer.name(`Layer ${this.layerArray.length + 1}`);
    }
    this.stage?.add(newLayer);
    this.layerArray.push(newLayer);
  }

  SelectLayer(event : MouseEvent){
    const name : string = (event.target as HTMLButtonElement).innerText;
    if(name){
      //console.log(`Selected layer should be: ${name}`);
      this.layerArray.forEach(layer => {
        if(layer.name() == name){
          this.selectedLayer = layer;
          //console.log(`Selected layer is: ${layer.name()}`);
          return;
        }
      });
    }
  }

  ObliterateLayer(name : string){
    //console.log("Layer for deletion: " + name);
    var targetedLayer = this.layerArray.find(layer => layer.name() == name);
    if(targetedLayer){
      var newArray : Konva.Layer[] = [];
      this.layerArray.forEach(layer => {
        if(layer != targetedLayer){
          newArray.push(layer);
        }
      });
      //console.log(`Old array length: ${this.layerArray.length}\nNew array length: ${newArray.length}`);
      targetedLayer.remove();
      //console.log(`Old array length: ${this.layerArray.length}\nNew array length: ${newArray.length}`);
      this.layerArray = newArray;
    }
  }

  BeginEdit(event : MouseEvent){
    event.stopPropagation();
    event.preventDefault();
    var targeted = event.target as HTMLSpanElement;
    if(targeted){
      var parent : HTMLElement = targeted.parentElement!;
      var inputField : HTMLInputElement = document.createElement('input') as HTMLInputElement;
      //inputField.classList.add('renaming-field'); mi az any..ért nem működsz, nincs nekem már ehhez türelmem
      inputField.style.maxWidth = '80%';
      inputField.style.maxHeight = '80%';
      inputField.style.color = 'black';
      inputField.placeholder = targeted.innerText;
      inputField.addEventListener('keyup', (event : KeyboardEvent) => {
        if(event.key === 'Enter'){
          this.EndEdit(event);
        }
      });
      if(parent){
        parent.appendChild(inputField);
      }
    }
    targeted.remove();
  }

  EndEdit(event : any){
    var targeted : HTMLInputElement = event.target as HTMLInputElement
    var inputValue : string = event.target.value as string;
    console.log(inputValue);
    if(targeted){
      var parent : HTMLElement = targeted.parentElement!;
      if(parent){
        var nameField : HTMLSpanElement = document.createElement('span') as HTMLSpanElement;
        if(inputValue && inputValue != "" && inputValue != ''){
          nameField.innerText = inputValue;
        } else {
          var index : number = this.FindIndexOfLayer(targeted.placeholder);
          nameField.innerText = `Layer ${index + 1}`;
          this.layerArray[index].name(`Layer ${index + 1}`);
        }
        nameField.addEventListener('click', event =>{
          this.BeginEdit(event);
        });
        parent.appendChild(nameField);
      }
    }
    targeted.remove();
  }

  FindIndexOfLayer(original : string) : number{
    var retval : number = -1;
    this.layerArray.forEach((layer, index) => {
      if(layer.name() == original){
        retval = index;
      }
    });
    return retval;
  }
}
