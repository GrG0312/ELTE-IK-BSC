import { Component, ViewChild } from '@angular/core';

@Component({
  selector: 'app-task1-a',
  templateUrl: './task1-a.component.html',
  styleUrls: ['./task1-a.component.less']
})
export class Task1AComponent {
  valueRed : number = 0;
  valueGreen : number = 0;
  valueBlue : number = 0;
  valueAplha : number = 1;

  BackGroundColor() : string{
    return `rgba(${this.valueRed}, ${this.valueGreen}, ${this.valueBlue}, ${this.valueAplha})`;
  }
}
