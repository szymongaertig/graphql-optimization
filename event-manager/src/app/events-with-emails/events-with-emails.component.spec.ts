import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventsWithEmailsComponent } from './events-with-emails.component';

describe('EventsWithEmailsComponent', () => {
  let component: EventsWithEmailsComponent;
  let fixture: ComponentFixture<EventsWithEmailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventsWithEmailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventsWithEmailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
