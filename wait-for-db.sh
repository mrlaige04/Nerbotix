#!/bin/bash

host="$1"
port="$2"
shift 2
cmd="$@"

until nc -z "$host" "$port"; do
    >&2 echo "Waiting for db..."
    sleep 1
done

exec $cmd