import { AngularWebAppV2Page } from './app.po';

describe('angular-web-app-v2 App', () => {
  let page: AngularWebAppV2Page;

  beforeEach(() => {
    page = new AngularWebAppV2Page();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
