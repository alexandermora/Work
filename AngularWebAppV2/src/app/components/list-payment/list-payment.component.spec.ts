import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListPaymentComponent } from './list-payment.component';

describe('ListPaymentComponent', () => {
  let component: ListPaymentComponent;
  let fixture: ComponentFixture<ListPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
