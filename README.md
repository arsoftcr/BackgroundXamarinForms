# BackgroundXamarinForms
Realizar tareas en segundo plano con xamarin forms

Instale el paquete nuget de la siguiente dirección:

Luego inicialize el plugin en cada plataforma, tanto en Android como en IOS de la siguiente manera:

Agregue las siguientes 2 clases tanto en el proyecto de Android como en el proyecto de IOS:


      public class BackgroundAggregator
    {
        public static void Init(ContextWrapper context)
        {
            MessagingCenter.Subscribe<StartLongRunningTask>(context, nameof(StartLongRunningTask), message =>
            {
                var intent = new Intent(context, typeof(BackgroundService));
                context.StartService(intent);
            });

            MessagingCenter.Subscribe<StopLongRunningTask>(context, nameof(StopLongRunningTask), message =>
            {
                var intent = new Intent(context, typeof(BackgroundService));
                context.StopService(intent);
            });
        }
    }
    
    
     [Service]
    public class BackgroundService : Service
    {
        private static bool _isRunning;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (!_isRunning)
            {

                BackgroundAggregatorService.Instance.Start();

                _isRunning = true;
            }

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            
        }
    }
    
    
Luego de eso agregue la siguiente línea en el MainActivity.cs del proyecto de Android y en el AppDelegate.cs del proyecto de IOS:
    
        BackgroundAggregator.Init(this);
    
Luego de esto implemente la interfaz en la clase   que contiene la tarea que desea realizar en segundo plano:
    
    
      public partial class Main : ContentPage, IBackgroundTask
    {
        
        public Main()
        {
            InitializeComponent();
        }

        public Task StartJob()
        {

            return Task.Run(() => {

               //Todo lo que se desea realizar en segundo plano
            });

        }


    }
    
    Y por último 2 líneas más en la case App.cs en el método OnStart() :
    
    
    protected override void OnStart()
        {
            BackgroundAggregatorService.Add(() => new Main());

           
            BackgroundAggregatorService.StartBackgroundService();
        }
    
    
    
