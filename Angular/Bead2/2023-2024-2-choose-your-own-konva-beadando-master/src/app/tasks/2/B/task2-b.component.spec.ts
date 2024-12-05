import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Task2BComponent } from 'src/app/tasks/2/B/task2-b.component';

describe('Task1BComponent', () => {
  let component: Task2BComponent;
  let fixture: ComponentFixture<Task2BComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Task2BComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Task2BComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
