import {Component, inject, model} from '@angular/core';
import {NZ_MODAL_DATA, NzModalRef} from "ng-zorro-antd/modal";

@Component({
  selector: 'app-preview',
  templateUrl: './preview.component.html',
  styleUrls: ['./preview.component.less']
})
export class PreviewComponent {
  constructor() {}

  modal = inject(NzModalRef);
  readonly data: { imageUrl: string, text: string } = inject(NZ_MODAL_DATA);
  protected readonly model = model;
}
