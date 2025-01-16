from flask import Flask, request, jsonify, send_file
from TTS.api import TTS
import os

# Initialize Flask app
app = Flask(__name__)

# Load TTS model
model_name = "tts_models/multilingual/multi-dataset/xtts_v2"
tts = TTS(model_name).to("cuda")

# Directory to save generated audio files
output_dir = "output_audio"
os.makedirs(output_dir, exist_ok=True)

@app.route('/generate', methods=['POST'])
def generate_audio():
    try:
        data = request.json
        text = data.get('text')
        voice = data.get('voice')

        if not text:
            return jsonify({"error": "Text is required"}), 400

        output_file = os.path.join(output_dir, "output.mp3")

        tts.tts_to_file(
            text=text,
            speaker_wav=voice+".mp3",
            file_path=output_file,
            language="en"
        )

        return send_file(output_file, as_attachment=True)

    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/health', methods=['GET'])
def health_check():
    return jsonify({"status": "Server is running"}), 200

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
