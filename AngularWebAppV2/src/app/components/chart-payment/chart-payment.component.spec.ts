import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartPaymentComponent } from './chart-payment.component';

describe('ChartPaymentComponent', () => {
  let component: ChartPaymentComponent;
  let fixture: ComponentFixture<ChartPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
