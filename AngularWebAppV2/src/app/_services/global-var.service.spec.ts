import { TestBed, inject } from '@angular/core/testing';

import { GlobalVarService } from './global-var.service';

describe('GlobalVarService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GlobalVarService]
    });
  });

  it('should be created', inject([GlobalVarService], (service: GlobalVarService) => {
    expect(service).toBeTruthy();
  }));
});
