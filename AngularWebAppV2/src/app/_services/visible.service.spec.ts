import { TestBed, inject } from '@angular/core/testing';

import { VisibleService } from './visible.service';

describe('VisibleService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [VisibleService]
    });
  });

  it('should be created', inject([VisibleService], (service: VisibleService) => {
    expect(service).toBeTruthy();
  }));
});
