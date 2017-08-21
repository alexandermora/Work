import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewDeleteComponent } from './preview-delete.component';

describe('PreviewDeleteComponent', () => {
  let component: PreviewDeleteComponent;
  let fixture: ComponentFixture<PreviewDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreviewDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
