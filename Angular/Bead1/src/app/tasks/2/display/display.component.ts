import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Worker, Permission } from 'src/app/tasks/2/A/task2-a.component';

@Component({
  selector: 'app-display',
  templateUrl: './display.component.html',
  styleUrl: './display.component.less'
})
export class DisplayComponent {
  @Input() worker! : Worker;
  @Input() redBg! : boolean;

  @Output() workerOut: EventEmitter<Worker> = new EventEmitter<Worker>();

  RemoveError(){
    this.worker.errors = this.worker.errors - 1;
  }
}
