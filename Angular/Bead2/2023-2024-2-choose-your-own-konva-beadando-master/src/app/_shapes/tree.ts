import Konva from "konva";
import {ShapeType} from "../_models/shapes";

export class Tree {
  x: number;
  y: number;
  width: number;
  height: number;
  draggable: boolean;


  constructor(x: number, y: number, width: number, height: number, draggable: boolean) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.draggable = draggable;
  }

  draw(layer: Konva.Layer): void {
    layer.add(this.shape());
  }

  shape(): Konva.Group {
    const group = new Konva.Group({
      draggable: this.draggable,
      type: ShapeType.TREE
    });
    const body = new Konva.Line({
      points: [
        this.x + this.width / 2,
        this.y,
        this.x,
        this.y + 4/5 * this.height,
        this.x + this.width,
        this.y + 4/5 * this.height
      ],
      closed: true,
      fill: 'green',
      strokeWidth: 0,
    });
    const leg = new Konva.Rect({
      x: this.x + 1/3 * this.width,
      y: this.y + 4/5 * this.height,
      width: 1/3 * this.width,
      height: 1/5 * this.height,
      fill: 'brown',
      strokeWidth: 0
    })

    group.add(body, leg);
    return group;
  }

}
