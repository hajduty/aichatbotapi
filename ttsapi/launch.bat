@echo off
:: Navigate to the project directory
cd /d "%~dp0"

:: Activate the virtual environment (if you have one)
:: Uncomment the next line and update "venv" if using a virtual environment
:: call venv\Scripts\activate

:: Install required dependencies
echo Installing dependencies...
pip install -r requirements.txt

:: Run main.py
echo Starting the Flask server...
python main.py

:: Pause to keep the window open after execution
pause
