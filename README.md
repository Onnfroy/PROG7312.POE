# PROG7312 Municipal Services Application

A C# .NET WinForms application developed as part of the PROG7312 Portfolio of Evidence (PoE).  
The application provides a simple, user-friendly interface for citizens to engage with municipal services.

---

## Features
- **Main Menu**
  - Report Issues (implemented)
  - Local Events & Announcements (coming soon…)
  - Service Request Status (coming soon…)
- **Report Issues**
  - Enter **location** of the issue
  - Select **category** (Sanitation, Roads, Water, Electricity, Public Safety, Other)
  - Enter a **detailed description**
  - Attach **files/images/documents** related to the issue
  - Submit report → shows progress bar + confirmation message
  - Dynamic **engagement strategy**: motivational messages rotate after submissions
- **Custom UI**
  - Soft beige theme with modern, minimal layout
  - Rounded buttons with hover + shadow effect
  - Scrolling banner text
  - Header image (Table Mountain) with title & subtitle
- **Custom Data Structures**
  - Issues and attachments stored in a **custom linked list**
  - Engagement messages stored in a **queue**
  - No built-in C# `List<>` used, to meet module requirements

---

## Tech Stack
- **Language:** C#  
- **Framework:** .NET Framework / WinForms  
- **IDE:** Visual Studio 2022  
- **Target:** C# 7.3 (compatibility with lab PCs)  

---

## Project Structure
/PROG7312.POE
├── DataStructures/    # Custom LinkedList, Queue, etc.
├── Models/            # Core models (Issue, Attachment, IssueCategory)
├── UI/                # Forms (MainForm, ReportIssueForm)
├── Resources/         # Images (Table Mountain)
├── App.config
├── Program.cs
└── README.md

---

## Running the Application
1. Clone the repository:
   ```bash
  git clone https://github.com/<onnfroy>/PROG7312.POE.git
  
  2.	Open the solution in Visual Studio 2022.
	3.	Set the startup project to PROG7312.POE.
	4.	Build and run (Ctrl + F5).
	5.	The main menu will appear:
	•	Select Report Issues to test the implemented functionality.

Video Demonstration

A short video has been recorded to demonstrate the application in action.
You can view it here: YouTube Demo Link

Documentation
	•	Research Report (Task 1) is included in the repository as a Word document (Task1_Research.docx).
	•	This document lists 5 user engagement strategies, with a detailed justification of the chosen one.

Notes
	•	Database integration is not required.
	•	All inputs are stored in memory via custom data structures.
	•	The “Local Events” and “Service Request Status” modules are coming soon (placeholders only).

⸻

Acknowledgements
	•	Module: PROG7312 — Advanced Application Development
	•	Institution: IIE
	•	Lecturer requirements:
	•	No use of built-in List<> or ArrayList (custom structures only).
	•	Professional UI/UX design.
	•	Full submission: Research report, application, README, and video demonstration.
	•	AI Assistance: Parts of this project were refined with the help of ChatGPT (conversation logs attached as per module guidelines).

 ---
