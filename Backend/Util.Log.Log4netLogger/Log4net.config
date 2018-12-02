<?xml version="1.0" encoding="utf-8" ?>
<!--
        .NET application configuration file
        This file must have the exact same name as your application with .config appended to it.

        For example if your application is ConsoleApp.exe then the config file must be ConsoleApp.exe.config.
        It must also be in the same directory as the application.
    -->
<configuration>
	<!-- Register a section handler for the log4net section -->
	<configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net" />
	</configSections>
	<log4net>
		<appender name="LocalLogAppender" type="log4net.Appender.RollingFileAppender">
			<file value=".\LocalLog.log" />
			<lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
			<appendToFile value="true" />
			<rollingStyle value="Date" />
			<datePattern value="yyyyMMdd" />
			<param name="PreserveLogFileNameExtension" value="true" />
			<maxSizeRollBackups value="10" />
			<maximumFileSize value="100MB" />
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%d [%t] %-5p - %m%n" />
			</layout>
			<encoding value="utf-8" />
		</appender>
		<!--<root>
      <priority value="ALL" />
      <appender-ref ref="RollingLogFileAppender" />
    </root>-->
		<logger name="DefaultLogger">
			<level value="Info" />
			<appender-ref ref="LocalLogAppender" />
		</logger>
	</log4net>
</configuration>