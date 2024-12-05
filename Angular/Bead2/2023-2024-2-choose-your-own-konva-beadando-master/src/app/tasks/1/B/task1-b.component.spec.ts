import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Task1BComponent } from './task1-b.component';

describe('Task1BComponent', () => {
  let component: Task1BComponent;
  let fixture: ComponentFixture<Task1BComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Task1BComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Task1BComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
