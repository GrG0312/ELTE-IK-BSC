import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Task2AComponent } from 'src/app/tasks/2/A/task2-a.component';

describe('Task1AComponent', () => {
  let component: Task2AComponent;
  let fixture: ComponentFixture<Task2AComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Task2AComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Task2AComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
