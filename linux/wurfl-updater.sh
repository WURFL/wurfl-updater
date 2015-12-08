#!/bin/sh

if [ "$#" -ne 2 ]; then
    echo "ScientiaMobile WURFL Snapshot Updater" >&2
    echo "This utility is used to update the WURFL.xml device database file." >&2
    echo "Note that the file format (zip or xml.gz) is determined by the URL." >&2
    echo "" >&2
    echo "usage: ./wurfl-updater.sh <url> <download_path>" >&2
    echo "  url            The WURFL Snapshot URL from your customer vault on scientiamobile.com" >&2
    echo "                 ex: https://data.scientiamobile.com/xxxxx/wurfl.zip" >&2
    echo "" >&2
    echo "  download_path  The directory to place the WURFL file into." >&2
    echo "" >&2

    exit 1
fi

# The WURFL Snapshot URL from ScientiaMobile
SNAPSHOT_URL="$1"

# The local directory for the wurfl.zip file
WURFL_DIR="$2"

WURFL_FILE=$(basename "$SNAPSHOT_URL")
WURFL_PATH="$WURFL_DIR/$WURFL_FILE"
LAST_DATE=$(date -r "$WURFL_PATH" --utc --rfc-2822 2>/dev/null || date --utc --rfc-2822 --date='1 week ago')
CURL=$(which curl 2> /dev/null)
WGET=$(which wget 2> /dev/null)

if [ "x$CURL" != "x" ]; then
    # Use cURL method
    echo "Updating $WURFL_PATH via CURL"
    $CURL -sSfRz "$LAST_DATE" -o "$WURFL_PATH" "$SNAPSHOT_URL"

elif [ "x$WGET" != "x" ]; then
    # Use wget method
    echo "Updating $WURFL_PATH via WGET"
    $WGET -N -P "$WURFL_DIR" --header="If-Modified-Since: $LAST_DATE" "$SNAPSHOT_URL"

else
    echo "No supported download method.  Please install 'curl' or 'wget'." >&2
    exit 2
fi
