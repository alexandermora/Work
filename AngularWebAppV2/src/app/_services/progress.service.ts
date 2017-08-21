import { Injectable } from '@angular/core';
import { BrowserXhr } from '@angular/http';
import { Subject } from 'rxjs/Rx';

@Injectable()
export class ProgressService
{
    private uploadProgress: Subject<any>;

    constructor()
    {

    }

    startTrackingUpload()
    {
        this.uploadProgress = new Subject();
        return this.uploadProgress;
    }
    notify(progress)
    {
        this.uploadProgress.next(progress);
    }
    endTrackingUpload()
    {
        this.uploadProgress.complete();
    }
}

@Injectable()
export class BroserXhrWithProgress extends BrowserXhr
{
    constructor(private service: ProgressService)
    {
        super();
    }

    build(): XMLHttpRequest
    {
        const xhr: XMLHttpRequest = super.build();

        xhr.upload.onprogress = (event) =>
        {
            this.service.startTrackingUpload().next(this.createProgress(event));
        };

        xhr.upload.onloadend = () =>
        {
            // Complete
            this.service.endTrackingUpload();
        };

        return xhr;
    }
  private createProgress(event)
  {
      return {
          total: event.total,
          percentage: Math.round(event.loaded / event.total * 100)
      };
  }
}
