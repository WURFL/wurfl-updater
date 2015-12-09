# wurfl-updater
Utilities for updating the ScientiaMobile WURFL device database file (WURFL.xml).

The files in this project are only useful for downloading the commercial WURFL Device Detection database updates and will not function without a licensed product from ScientiaMobile that is eligible for on-premise updates.  Please see http://www.scientiamobile.com for details.

## Linux / UNIX / Mac OS X
The `linux/` folder contains a script that can be used to update the WURFL file on Linux, UNIX, Mac OS X and perhaps other UNIX-like environments.

To see the usage information, run `./wurfl-uploader.sh` with no arguments:

    $ linux/wurfl-updater.sh
    ScientiaMobile WURFL Snapshot Updater
    This utility is used to update the WURFL.xml device database file.
    Note that the file format (zip or xml.gz) is determined by the URL.

    usage: ./wurfl-updater.sh <url> <download_path>
      url            The WURFL Snapshot URL from your customer vault on scientiamobile.com
                     ex: https://data.scientiamobile.com/xxxxx/wurfl.zip

      download_path  The directory to place the WURFL file into.

> Note: the filename will be the last segment of the snapshot URL (either `wurfl.zip` or `wurfl.xml.gz`)

To download or update your WURFL file, pass the WURFL Snapshot URL from your customer vault on ScientiaMobile.com as the first argument and the directory that you want the file to be downloaded into as the second argument.

When you run the updater, if there is not a local copy of the file, the file is downloaded.  If there is a local copy of the file but it is out of date, the updated version is downloaded, otherwise nothing is downloaded.

### Requirements
This script should run on must UNIX-like systems with no additional packages, however, you will need `curl` or `wget`, as well as `date`, `grep` and `stat`.

### Examples

	# Success or already up-to-date
    $ ./wurfl-updater.sh https://data.scientiamobile.com/xxxxx/wurfl.zip /tmp
    Updating /tmp/wurfl.zip via CURL
	
	# URL or License is invalid
	$ ./wurfl-updater.sh https://data.scientiamobile.com/xxxxx/wurfl.zip /tmp
    Updating /tmp/wurfl.zip via CURL
    curl: (22) The requested URL returned error: 402 Payment Required

> The URL above is fake and it will not work - you must have an eligible product from ScientiaMobile and use the URL from your customer vault.

## Windows

For Windows users there is a Visual Studio project that can be used to generate the updater `exe` file.  This program will work exactly the same way as the Linux script, with the same command-line arguments.

### Examples

	# Success
    C:\temp\WURFLUpdater.exe https://data.scientiamobile.com/xxxxx/wurfl.zip c:\temp
    Downloading WURFL file...
    Successfully downloaded WURFL to: c:\temp\wurfl.zip

	# File is already up-to-date
    C:\temp\WURFLUpdater.exe https://data.scientiamobile.com/xxxxx/wurfl.zip c:\temp
    Downloading WURFL file...
    The WURFL File is up to date.

	# URL or License is invalid
    C:\temp\WURFLUpdater.exe https://data.scientiamobile.com/xxxxx/wurfl.zip c:\temp
    Downloading WURFL file...
    Unexpected status code: HTTP 402: Payment Required

