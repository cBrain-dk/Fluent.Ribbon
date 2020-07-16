namespace FluentTest
{
    using System.Windows;
    using Fluent;
    using FluentTest.Icons;

    public partial class App
    {
        public App()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");

            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hu");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("cs");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-br");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sv");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sk");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("uk");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ro");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("it");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("da");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("az");

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            ThemeManager.SyncThemeWithWindowsAppModeSetting();
            ThemeManager.AddTheme(new System.Uri("pack://application:,,,/Fluent.Ribbon.Showcase;component/Themes/Light.F2.xaml"));
            ThemeManager.AddTheme(new System.Uri("pack://application:,,,/Fluent.Ribbon.Showcase;component/Themes/Dark.F2.xaml"));

            ThemeManager.ChangeTheme(Application.Current, ThemeManager.GetTheme("F2.Standard"));
            Fluent.Converters.ObjectToImageConverter.CustomObjectToImagerConverter = new CustomImageConverter();

            base.OnStartup(e);
        }
    }
}