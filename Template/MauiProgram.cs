﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Template.Interfaces;
using Template.Platforms.Source;

namespace Template;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("WorkSans-Regular.ttf", "WorkSans-Regular");
                fonts.AddFont("WorkSans-SemiBold.ttf", "WorkSans-SemiBold");
                fonts.AddFont("WorkSans-Medium.ttf", "WorkSans-Medium");
                fonts.AddFont("WorkSans-Bold.ttf", "WorkSans-Bold");
            });

        builder.Services.AddTransient<IPathService, PathService>();
        builder.Services.AddTransient<ILocalize, Localize>();
        builder.Services.AddTransient<IDeviceOrientationService, DeviceOrientation>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

