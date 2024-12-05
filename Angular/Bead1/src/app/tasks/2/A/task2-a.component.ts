import { Component } from '@angular/core';

// Enum defining permission types
export enum Permission {
  WRITER = 'WRITER',
  READER = 'READER',
  ADMIN = 'ADMIN'
}

// Class representing a Worker
export class Worker {
  name : string;
  errors: number;
  permission : Permission;
  
  constructor(
    name: string,
    errors: number,
    permission: Permission
  ) {
    this.name = name;
    this.errors = errors;
    this.permission = permission;
  }
}

@Component({
  selector: 'app-task2-a',
  templateUrl: './task2-a.component.html',
  styleUrls: ['./task2-a.component.less']
})
export class Task2AComponent {
  workerIn? : Worker;
  switchValue : boolean = false;
  workers : Worker[] = [
    new Worker('Rex', 1, Permission.WRITER),
    new Worker('Cody', 2, Permission.WRITER),
    new Worker('Echo', 5, Permission.READER),
    new Worker('Fives', 7, Permission.READER),
    new Worker('Skywalker', 11, Permission.ADMIN),
    new Worker('Grievous', 20, Permission.ADMIN),
    new Worker('Yoda', 10, Permission.ADMIN)
  ];

  RefreshWorker(w : Worker){
    this.workers.forEach((value : Worker, index : number) => {
      if(value.name == w.name){
        value.errors = w.errors;
      }
    });
  }
}
