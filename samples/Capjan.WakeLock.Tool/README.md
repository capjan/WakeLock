# Capjan.WakeLock.Tool

`dotnet tool` to keep the current machine awake until a key is pressed.

## Install

```bash
dotnet tool install --global Capjan.WakeLock.Tool
```

## Run

```bash
wakelock
```

Use `--display` (or `-d`) to keep both system sleep and display sleep disabled:

```bash
wakelock --display
```

Show help:

```bash
wakelock --help
```

Show version:

```bash
wakelock --version
```
