import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationsListComponent } from './registrations-list.component';

describe('RegistrationsListComponent', () => {
  let component: RegistrationsListComponent;
  let fixture: ComponentFixture<RegistrationsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegistrationsListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
