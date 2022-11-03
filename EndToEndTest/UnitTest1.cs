using Microsoft.Playwright;

namespace EndToEndTest
{
    public class UnitTest1 : IClassFixture<PlaywrightFixture>
    {
        private IBrowser Browser { get; }

        public UnitTest1(PlaywrightFixture fixture)
        {
            Browser = fixture.Browser;
        }

        [Fact]
        [Trait("Category", "Manual")]
        public async Task Google_Im_Feeling_lucky()
        {
            var page = await Browser.NewPageAsync();

            await page.GotoAsync("https://teams.microsoft.com");
            await page.Locator(@"[placeholder=""Email\, phone\, or Skype""]").FillAsync("lol");

            var locator = page.Locator("input#gbqfbb");
            var actual = await locator.CountAsync();
            await Task.Delay(1000000);
            Assert.Equal(1, actual);
        }
    }
}