import Konva from "konva";
import {ShapeType} from "../_models/shapes";

export class FemaleNPC {
  x: number;
  y: number;
  width: number;
  height: number;
  draggable: boolean;

  constructor(x: number, y: number, width: number, height: number, draggable: boolean = true) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.draggable = draggable;
  }

  draw(layer: Konva.Layer) {
    layer.add(this.shape());
  }

  shape(): Konva.Group {
    const group = new Konva.Group({
      draggable: this.draggable,
      type: ShapeType.FEMALE_NPC
    });
    const body = new Konva.Line({
      points: [
        this.x,
        this.y + this.height,
        this.x + this.width,
        this.y + this.height,
        this.x + this.width / 2,
        this.y + 3/5 * this.height,
      ],
      closed: true,
      fill: 'pink',
      stroke: 'black',
      strokeWidth: 1
    });
    const head = new Konva.Circle({
      x: this.x + this.width / 2,
      y: this.y + 2/5 * this.height,
      radius: this.height / 5,
      stroke: 'black',
      strokeWidth: 1,
    })

    group.add(body, head)
    return group;
  }
}
