import { Component } from '@angular/core';
import {RouterService} from "src/app/_services/router.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'choose-your-own-beadando';

  constructor(public routerService: RouterService) {
  }

}
