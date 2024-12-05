import { Component, OnInit } from '@angular/core';
import {tasks} from "src/app/configuration/tasks";
import {ActivatedRoute} from "@angular/router";
import {RouterService} from "src/app/_services/router.service";
import {PreviewComponent} from "src/app/preview/preview.component";
import {NzModalService} from "ng-zorro-antd/modal";

@Component({
  selector: 'app-navigator',
  templateUrl: './navigator.component.html',
  styleUrls: ['./navigator.component.less']
})
export class NavigatorComponent implements OnInit {

  currentIndex: number = 1;
  selectedTask?: 'A' | 'B';
  tasks = tasks;
  constructor(private modalService: NzModalService,
              private route: ActivatedRoute,
              private routerService: RouterService) {}

  ngOnInit(): void {
    this.route.params
      .subscribe((params) => {
        if (params['index']) {
          this.currentIndex = +params['index'];
        }
        if (params['taks']) {
          this.selectedTask = params['task'];
        }
      });
    if (this.route.snapshot.params['index']) {
      this.currentIndex = +this.route.snapshot.params['index'];
    }
    if (this.route.snapshot.params['task']) {
      this.selectedTask = this.route.snapshot.params['task'];
    }
  }

  nextTask() {
    this.currentIndex += 1;
    // currentIndex starts from 1 not 0
    if (this.tasks[this.currentIndex - 1] && this.tasks[this.currentIndex - 1].preview) {
      console.log('has preview');

      this.modalService.create({
        nzTitle: undefined,
        nzFooter: null,
        nzContent: PreviewComponent,
        nzData: {
          imageUrl: this.tasks[this.currentIndex - 1].preview?.imageUrl,
          text: this.tasks[this.currentIndex - 1].preview?.text,
        }
      });
    }
    if (this.currentIndex <= tasks.length) {
      this.selectedTask = undefined;
      this.routerService.routeToTaskWithoutReload(this.currentIndex, undefined);
    } else {
      this.routerService.routeToSummary();
    }
  }

  selectTask(task?: 'A' | 'B') {
    this.selectedTask = task;
    this.routerService.routeToTaskWithoutReload(this.currentIndex, task);
  }

}
