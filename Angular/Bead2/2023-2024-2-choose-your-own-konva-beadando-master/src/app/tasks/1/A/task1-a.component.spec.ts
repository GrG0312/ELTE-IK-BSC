import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Task1AComponent } from './task1-a.component';

describe('Task1AComponent', () => {
  let component: Task1AComponent;
  let fixture: ComponentFixture<Task1AComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Task1AComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Task1AComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
