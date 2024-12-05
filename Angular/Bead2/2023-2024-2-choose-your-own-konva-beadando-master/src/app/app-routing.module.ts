import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {NavigatorComponent} from "src/app/navigator/navigator.component";
import {PageRoutes} from "src/app/_constants/page-routes";
import {SummaryComponent} from "src/app/summary/summary.component";

const routes: Routes = [
  {
    path: PageRoutes.summary,
    component: SummaryComponent
  },
  {
    path: `${PageRoutes.task}/:index/:task`,
    component: NavigatorComponent
  },
  {
    path: `${PageRoutes.task}/:index`,
    component: NavigatorComponent
  },
  {
    path: PageRoutes.task,
    component: NavigatorComponent
  },
  {
    path: '',
    redirectTo: PageRoutes.task,
    pathMatch: 'prefix'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
