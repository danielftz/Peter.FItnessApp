namespace FitnessApp.Tool;

#if ANDROID
using Android.Media;
using Android.Net;
using Android.Content.Res;

public class Alarm
{
	public static Alarm Instance
	{
		get
		{
			_instance ??= new Alarm();
			return _instance;
		}
	}
	private static Alarm? _instance;

	private readonly MediaPlayer? _mediaPlayer;

	private Alarm()
	{
		_mediaPlayer = new MediaPlayer() { Looping = false };
		AssetFileDescriptor? afd = Android.App.Application.Context.Assets?.OpenFd("alarm.mp3");
		if (afd is not null)
		{
			_mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
			_mediaPlayer.Prepare();
		}
	}

	public void Start()
	{
        _mediaPlayer?.Start();
	}

    public void Stop()
    {
        _mediaPlayer?.Pause();
        _mediaPlayer?.SeekTo(0);
    }
}

#elif IOS
using AVFoundation;
using Foundation;
public class Alarm
{
	public static Alarm Instance
	{
		get
		{
			_instance ??= new Alarm();
			return _instance;
		}
	}
	private static Alarm? _instance;

	private readonly AVAudioPlayer? _mediaPlayer;

	private Alarm()
	{
		_mediaPlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename("alarm.mp3"));
		if (_mediaPlayer is not null)
		{
			_mediaPlayer.PrepareToPlay();
		}
		
	}

	public void Start()
	{
        _mediaPlayer?.Play();
	}

	public void Stop()
	{
        _mediaPlayer.CurrentTime = 0;
        _mediaPlayer?.Pause();
        
    }
}
#endif

