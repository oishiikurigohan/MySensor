using System.Threading;
using Xamarin.Forms;
using CocosSharp;

namespace MySensor
{
    public partial class MainPage : ContentPage
    {
        private CCAccelerometer accelerometer;
        private Timer timer;

        public MainPage()
        {
            InitializeComponent();

            var cocossharpView = new CocosSharpView();
            cocossharpView.ViewCreated = (sender, e) =>
            {
                var ccgameView = sender as CCGameView;
                var scene = new CCScene(ccgameView);

                accelerometer = new CCAccelerometer(ccgameView);
                accelerometer.Enabled = true;

                var listener = new CCEventListenerAccelerometer();
                listener.OnAccelerate = DidAccelerate;
                scene.AddEventListener(listener);

                var callBack = new TimerCallback((o)=> { accelerometer.Update(); });
                timer = new Timer(callBack, null, Timeout.Infinite, Timeout.Infinite);
                ccgameView.RunWithScene(scene);
            };
            MyGrid.Children.Add(cocossharpView, 0, 0);

            StartButton.Clicked += (sender, e) => timer.Change(0, 1000);
            StopButton.Clicked += (sender, e) => timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void DidAccelerate(CCEventAccelerate AccelEvent)
        {
            Device.BeginInvokeOnMainThread(() => {
                Xaxis.Text = AccelEvent.Acceleration.X.ToString();
                Yaxis.Text = AccelEvent.Acceleration.Y.ToString();
                Zaxis.Text = AccelEvent.Acceleration.Z.ToString();
                Ticks.Text = AccelEvent.Acceleration.TimeStamp.ToString();
            });
        }
    }
}
