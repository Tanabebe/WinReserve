using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace WinReserve
{
    public class Execute
    {
        private static int _counter = 0;
        private readonly string _firstNavUrl;
        private readonly string _email;
        private readonly string _password;
        private readonly string _reserveNowtext;
        private readonly string _selenuimHub;

        /// <summary>
        /// 構成ファイルを読み込むコンストラクタ
        /// </summary>
        /// <param name="config">アプリケーション構成プロパティ</param>
        public Execute(IConfiguration config)
        {
            var baseJson = config.GetSection("web");
            _firstNavUrl = baseJson.GetSection("first_nav_url").Value;
            _email = baseJson.GetSection("user_email").Value;
            _password = baseJson.GetSection("user_password").Value;
            _reserveNowtext = baseJson.GetSection("reserve_now_text").Value;
            _selenuimHub = baseJson.GetSection("selenuim_hub").Value;
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        public void DoRun()
        {
            var options = new ChromeOptions();
            options.AddArguments(
                "--headless",
                "--start-maximized"
            );

            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "sc/");
            var driver = new RemoteWebDriver(new Uri(_selenuimHub), options);
            try
            {
                driver.Navigate().GoToUrl(_firstNavUrl);
                SaveScreenShot(driver, path);
                driver.FindElement(By.LinkText(_reserveNowtext)).Click();

                SaveScreenShot(driver, path);
                driver.FindElementById("user_email").SendKeys(_email);

                SaveScreenShot(driver, path);
                driver.FindElementById("_password").SendKeys(_password);

                SaveScreenShot(driver, path);
                driver.FindElement(By.Name("commit")).Submit();

                SaveScreenShot(driver, path);
                driver.FindElement(By.Name("commit")).Submit();

                SaveScreenShot(driver, path);
                driver.FindElement(By.Name("commit")).Submit();

                SaveScreenShot(driver, path);
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"対象の要素が存在しません\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        /// <summary>
        /// 画像名に使用する用のざっくりカウンター
        /// </summary>
        /// <returns>カウント+1</returns>
        public static int ImageCounter()
        {
            _counter++;
            return _counter;
        }

        /// <summary>
        /// 指定されたパスに画像を保存
        /// </summary>
        /// <param name="driver">RemoteWebDriver</param>
        /// <param name="path">保存先パス</param>
        public static void SaveScreenShot(RemoteWebDriver driver, string path)
        {
            driver.GetScreenshot().SaveAsFile($"{path}/{ImageCounter()}.png", ScreenshotImageFormat.Png);
        }
    }
}
