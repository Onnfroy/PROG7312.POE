# PROG7312 Municipal Services Application

A C# .NET WinForms application developed as part of the PROG7312 Portfolio of Evidence (PoE).  
The application provides a simple, user-friendly interface for citizens to engage with municipal services.

---

## Features

### Main Menu
- **Report Issues** (implemented)
- **Local Events & Announcements** (implemented in Part 2)
- **Service Request Status** (placeholder for Part 3)
- Themed UI, centered header, Table Mountain image, and animated marquee banner.

### Report Issues (Part 1)
- Enter **location**, select **category** (Sanitation, Roads, Water, Electricity, Public Safety, Other), and provide a **detailed description**.
- Attach **files/images/documents**.
- Progress animation + rotating **engagement messages**.
- Data stored in **custom data structures**:
  - `LinkedList<Issue>` for all submissions (custom implementation).
  - `Queue<string>` for the engagement messages (custom implementation).

### Local Events & Announcements (Part 2)
- **Display**: clean ListView with columns (Title, Date, Category, Location, Description).
- **Search & Filter**:
  - Filter by **Category**.
  - Filter by **Date**.
  - Search by **Title** (live substring match).
- **Sort**: by Date ↑, Date ↓, Title, or Category (manual insertion/compare).
- **Recommendations**:
  - Tracks the **last** search action (date/category/title) and shows a friendly hint.
  - Highlights matching rows to give immediate visual feedback.
- **New Event Submissions**:
  - **Add Sample Submission** puts an event into a **custom `Queue<EventItem>`** (FIFO).
  - **Process Next** dequeues and adds it to the store; category list updates automatically.
  - Footer shows **Queued: N**.
- **Last Viewed**:
  - Remembers the most recently selected event and pops it back using a stack-like behavior on a **custom `LinkedList<Guid>`**.

---

## Data Structures (Module Requirements)

- **Primary event storage**:  
  `SortedDictionary<DateTime, LinkedList<EventItem>>`  
  Each date bucket uses the **custom `LinkedList<T>`** (no `List<>` used for storage).
- **Unique categories**:  
  `HashSet<string>` (fast membership for combo/filter).
- **Queue for submissions**:  
  Custom `Queue<EventItem>` (backed by the custom linked list).
- **Stack-like last viewed**:  
  Custom `LinkedList<Guid>` with Push/Pop behavior.
- **Recommendation analytics**:  
  `Dictionary<string,int>` with a simple “last search key” to drive the UI label.

> Note: The app avoids built-in `List<>`/**ArrayList** for core storage as per module rules. Where BCL collections are used (e.g., `SortedDictionary`/`HashSet`), they’re permitted by the brief to demonstrate maps/sets.

---

## Tech Stack
- **Language:** C#
- **Framework:** .NET Framework / WinForms
- **IDE:** Visual Studio 2022
- **Target:** C# 7.3 (compatibility with lab PCs)

---

## Project Structure

/PROG7312.POE
├── DataStructures/           # Custom LinkedList, Queue
├── Models/                   # Issue, Attachment, IssueCategory, EventItem, EventCategory
├── Services/                 # IssueRepository, EventStore (SortedDictionary, HashSet, analytics)
├── UI/                       # MainForm, ReportIssueForm, EventsForm
├── Resources/                # Images (TableMountain.jpg)
├── App.config
├── Program.cs
└── README.md

---

## Running the Application

1. **Clone** the repository:
   ```bash
   git clone https://github.com/Onnfroy/PROG7312.POE.git

	2.	Open the solution in Visual Studio 2022.
	3.	Set startup project to PROG7312.POE.
	4.	Build & Run (Ctrl+F5).

What to try (quick test plan)
	•	Report Issues
	•	Add location, category, description.
	•	Attach a file and Submit → progress animation + rotating message (queue).
	•	Local Events & Announcements
	•	Use Category, Date, and Title to filter.
	•	Change Sort and watch the list reorder.
	•	Check the Recommendations label and highlighted rows (reflects your last search).
	•	Click Add Sample Submission a couple of times → Queued: N increments.
	•	Click Process Next → item dequeued and added; list updates; categories update if new.
	•	Select a few rows, then click Last Viewed to navigate back via stack behavior.

Video Demonstration

A short video showcases Part 1 and Part 2 features end-to-end:
YouTube: https://youtu.be/Y8KAkm3llis

Notes
	•	Everything is in-memory using custom data structures; no database required.
	•	The UI maintains a consistent theme, hover effects, and clear visual feedback for all interactions.
	•	“Service Request Status” remains a placeholder for the next phase.

⸻

Acknowledgements
	•	Module: PROG7312 — Advanced Application Development
	•	Institution: IIE
	•	Lecturer requirements:
	•	Avoid built-in List<>/ArrayList for core storage (custom structures implemented).
	•	Demonstrate maps/sets, queues/stacks, and user-facing algorithms.
	•	Clean, professional UI/UX and a working demo.
	•	AI Assistance: Portions of the refactoring and documentation were assisted by ChatGPT; conversation notes are available if required.
