import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Task} from '../_interfaces/task';
import {configuration} from "src/app/configuration/configuration";
@Component({
  selector: 'app-task-selector',
  templateUrl: './task-selector.component.html',
  styleUrls: ['./task-selector.component.less']
})
export class TaskSelectorComponent implements OnInit {
  @Input() ATask!: Task;
  @Input() BTask!: Task;
  @Input() index!: number;
  @Input() selectedTask?: 'A' | 'B';
  @Output() taskSelected: EventEmitter<'A' | 'B'> = new EventEmitter<'A' | 'B'>();

  configuration = configuration;

  constructor() { }

  ngOnInit(): void {
  }

}
