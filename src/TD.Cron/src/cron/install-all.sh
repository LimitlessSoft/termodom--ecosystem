for dir in src/cron/* src/cron/node-shared/*; do
  if [ -d "$dir" ] && [ "$(basename "$dir")" != "node-shared" ] && [ "$(basename "$dir")" != "install-all.sh" ]; then
    echo "Instaliranje zavisnosti u $dir"
    (cd "$dir" && npm install)
  fi
done