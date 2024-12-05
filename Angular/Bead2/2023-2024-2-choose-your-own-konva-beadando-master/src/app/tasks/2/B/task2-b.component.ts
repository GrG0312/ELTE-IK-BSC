import {AfterViewInit, Component, ElementRef} from '@angular/core';
import Konva from "konva";
import {Task2BKonvaMode} from "./models/task-2b-mode.model";
import {CarShape} from "../../../_shapes/car";
import {ParkingShape} from "../../../_shapes/parking";

@Component({
  selector: 'app-task2-b',
  templateUrl: './task2-b.component.html',
  styleUrls: ['./task2-b.component.less']
})
export class Task2BComponent implements AfterViewInit {
  selectedMode: Task2BKonvaMode = Task2BKonvaMode.CAR;
  selectedLayer?: Konva.Layer;
  stage?: Konva.Stage;

  Task2BKonvaMode = Task2BKonvaMode;

  constructor(private el: ElementRef) { }

  ngAfterViewInit() {
    setTimeout(() => { // Forcing a single change detection cycle delay
      this.stage = new Konva.Stage({
        container: 'konva-container',
        width: this.el.nativeElement.offsetWidth,
        height: 500,
      });
      const layer1 = new Konva.Layer();
      const layer2 = new Konva.Layer();
      this.stage.add(layer1);
      this.stage.add(layer2);

      this.selectedLayer = this.stage.getLayers()[0];

      this.stage.on('click', (event) => {
        if (this.stage) {
          let pointer = this.stage.getPointerPosition();
          if (pointer && event.target instanceof Konva.Stage) {
            switch (this.selectedMode) {
              case Task2BKonvaMode.CAR:
                const car = new CarShape(
                  this.stage,
                  pointer?.x,
                  pointer?.y,
                  50,
                  25,
                  true
                );
                this.selectedLayer?.add(car.shape());
                break;
              case Task2BKonvaMode.PARKING:
                const parking = new ParkingShape(
                  this.stage,
                  pointer?.x,
                  pointer?.y,
                  50,
                  50,
                  false
                );
                this.selectedLayer?.add(parking.shape());
                break;
            }
          }
        }
      });

    });

  }
}
