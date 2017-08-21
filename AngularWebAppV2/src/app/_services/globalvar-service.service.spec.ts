import { TestBed, inject } from '@angular/core/testing';

import { GlobalvarServiceService } from './globalvar-service.service';

describe('GlobalvarServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GlobalvarServiceService]
    });
  });

  it('should be created', inject([GlobalvarServiceService], (service: GlobalvarServiceService) => {
    expect(service).toBeTruthy();
  }));
});
