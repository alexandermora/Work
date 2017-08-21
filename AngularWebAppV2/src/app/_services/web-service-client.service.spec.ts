import { TestBed, inject } from '@angular/core/testing';

import { WebServiceClientService } from './web-service-client.service';

describe('WebServiceClientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WebServiceClientService]
    });
  });

  it('should be created', inject([WebServiceClientService], (service: WebServiceClientService) => {
    expect(service).toBeTruthy();
  }));
});
