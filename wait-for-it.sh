# wait-for-it.sh
#!/bin/bash
# wait-for-it.sh: Wait for the SQL Server to be available

set -e

host="$1"
port="$2"
shift 2
cmd="$@"

echo "Waiting for SQL Server ($host:$port) to be ready..."

# Timeout after 30 attempts
for i in {1..30}; do
  # Check if the port is open
  (echo > /dev/tcp/$host/$port) >/dev/null 2>&1
  result=$?
  if [ $result -eq 0 ]; then
    echo "SQL Server is ready after $i tries."
    exec $cmd
    break
  fi
  echo "Waiting ($i/30)..."
  sleep 2
done

if [ $result -ne 0 ]; then
  echo "SQL Server did not become ready within the timeout period."
  exit 1
fi