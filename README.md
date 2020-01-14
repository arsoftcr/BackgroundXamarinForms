# BackgroundXamarinForms
Realizar tareas en segundo plano con xamarin forms

Instale el paquete nuget en visual studio o por medio de consola : Install-Package BackgroundXamarinForms -Version 1.1.1

Luego inicialize el plugin en cada plataforma, tanto en Android como en IOS de la siguiente manera:

Agregue las siguientes 2 clases  en el proyecto de Android 


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
    
Agregue las siguientes 2 clases en el proyecto de IOS:

            public class BackgroundService
                {
                    private static nint _taskId;
                    private static BackgroundService _instance;
                    private static bool _isRunning;

                    static BackgroundService()
                    {
                    }

                    private BackgroundService()
                    {
                    }


                    public static BackgroundService Instance { get; } =
                        _instance ?? (_instance = new BackgroundService());


                    public void Start()
                    {
                        if (_isRunning) return;

                        //We only have 3 minutes in the background service as per iOS 9
                        _taskId = UIApplication.SharedApplication.BeginBackgroundTask(nameof(StartLongRunningTask), Stop);
                        BackgroundAggregatorService.Instance.Start();

                        _isRunning = true;
                    }

                    public void Stop()
                    {
                    }
                }
                
                
                
                 public class BackgroundAggregator
                  {
                    public static void Init(FormsApplicationDelegate appDelegate)
                    {
                        MessagingCenter.Subscribe<StartLongRunningTask>(appDelegate, nameof(StartLongRunningTask),
                            message => { BackgroundService.Instance.Start(); });
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
    
    
    
