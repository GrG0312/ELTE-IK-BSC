import {AfterViewInit, Component, ElementRef} from '@angular/core';
import {Task1BKonvaMode} from "./models/task-1b-mode.model";
import Konva from "konva";

@Component({
  selector: 'app-task1-b',
  templateUrl: './task1-b.component.html',
  styleUrls: ['./task1-b.component.less']
})
export class Task1BComponent implements AfterViewInit {
  selectedMode: Task1BKonvaMode = Task1BKonvaMode.SELECT;
  selectedLayer?: Konva.Layer;
  stage?: Konva.Stage;
  transformer?: Konva.Transformer;

  Task1BKonvaMode = Task1BKonvaMode;
  constructor(private el: ElementRef) { }

  ngAfterViewInit() {
    setTimeout(() => { // Forcing a single change detection cycle delay
      this.stage = new Konva.Stage({
        container: 'konva-container',
        width: this.el.nativeElement.offsetWidth,
        height: 500,
      });
      const layer = new Konva.Layer();
      this.stage.add(layer);

      this.selectedLayer = this.stage.getLayers()[0];

      this.transformer = new Konva.Transformer();
      this.selectedLayer.add(this.transformer);

      this.stage.on('click', (event) => {
        if (this.stage) {
          let pointer = this.stage.getPointerPosition();
          if (event.target instanceof Konva.Stage) {
            switch (this.selectedMode) {
              case Task1BKonvaMode.RECTANGLE:
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
              case Task1BKonvaMode.SELECT:
                this.transformer?.nodes([]);
                break;
            }
          } else {
            switch (this.selectedMode) {
              case Task1BKonvaMode.SELECT:
                this.transformer?.nodes([event.target]);
                break;
            }
          }

        }
      });

    });

  }
}
