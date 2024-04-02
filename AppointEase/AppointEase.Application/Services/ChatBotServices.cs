using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using Microsoft.AspNetCore.Identity;


public class ChatBotService
{
    private readonly IDoctorService _doctorService;
    private readonly IClinicService _clinicService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChatBotService(IDoctorService doctorService, IClinicService clinicService, UserManager<ApplicationUser> userManager )
    {
        _doctorService = doctorService;
        _clinicService = clinicService;
        _userManager = userManager;
    }

    public object GenerateResponse(ChatbotRequest chatbotRequest)
    {
        string option = chatbotRequest.Options ?? "";
        if(chatbotRequest.FreeTextQuestion != string.Empty)
        {
            return HandleFreeQuestions(chatbotRequest);
        }
        else
        {
            switch (chatbotRequest.Problem)
            {
                case "Technical Support":
                    return HandleTechnicalSupport(option);
                case "Problem Solution":
                    return HandleProblemSolution(option);
                case "Find a Doctor":
                    return HandleFindDoctor(option);
                case "Medical Assistance":
                    return HandleMedicalAssistance(option);
                default:
                    return new { message = "I'm sorry, I'm not sure how to help with that at the moment.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            }
        }
        
    }


    #region FreeQuestions

    private readonly string[] morningResponses = {
        "Good morning! How can I assist you today?",
        "Good morning! What can I help you with?",
        "Good morning! How may I be of service to you?"
    };

    private readonly string[] afternoonResponses = {
        "Good afternoon! How can I assist you today?",
        "Good afternoon! What can I help you with?",
        "Good afternoon! How may I be of service to you?"
    };

    private readonly string[] eveningResponses = {
        "Good evening! How can I assist you today?",
        "Good evening! What can I help you with?",
        "Good evening! How may I be of service to you?"
    };

    private readonly string[] genericResponses = {
        "Hello! How can I assist you today?",
        "Hi there! What can I help you with?",
        "Hey! How may I be of service to you?",
        "Greetings! How can I assist you?",
        "Good day! How may I help you today?"
    };

    private readonly string[] morningGreetings = { "good morning", "morning", "morn" };
    private readonly string[] afternoonGreetings = { "good afternoon", "afternoon" };
    private readonly string[] eveningGreetings = { "good evening", "evening" };
    private readonly string[] genericGreetings = { "hello", "hi", "hey", "good day", "greetings" };



    private object HandleFreeQuestions(ChatbotRequest chatbotRequest)
    {
        string question = chatbotRequest.FreeTextQuestion.ToLower();

        if (IsGreeting(question))
        {
            return new { message = GenerateGreetingResponse(chatbotRequest), date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString(),isFreeQuestion = true };
        }

        if (question.Contains("help") || question.Contains("assist") || question.Contains("support"))
        {
            return new { message = "I'm here to assist you. How may I help you today?", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString(), isFreeQuestion = true };
        }
        else if (question.Contains("purpose") && (question.Contains("application") || question.Contains("app")))
        {
            return new { message = "The purpose of this application is to provide users with comprehensive medical assistance, technical support, and other related services.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString(), isFreeQuestion = true };
        }
        else if (question.Contains("contact") || question.Contains("reach") || question.Contains("connect"))
        {
            return new { message = "For any further assistance or inquiries, please reach out to our support team at support@example.com or call us at +1-123-456-7890.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString(), isFreeQuestion = true };
        }
        else if (question.Contains("availability") || question.Contains("hours") || question.Contains("open"))
        {
            return new { message = "Our services are available 24/7 to assist you. Feel free to reach out to us anytime.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString(), isFreeQuestion = true };
        }
        else
        {
            return new { message = "I apologize, but I'm not able to understand your question at the moment. Please provide more details or try asking in a different way.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString(), isFreeQuestion = true };
        }
    }

    public object GenerateGreetingResponse(ChatbotRequest chatbotRequest)
    {
        string question = chatbotRequest.FreeTextQuestion?.ToLower() ?? "";
        DateTime now = DateTime.Now;
        string[] responses;

        if (IsMorning(now))
            responses = morningResponses;
        else if (IsAfternoon(now))
            responses = afternoonResponses;
        else
            responses = eveningResponses;

        if (IsGreeting(question))
            return GetRandomResponse(responses, now);


        return GetRandomResponse(genericResponses, now);
    }

    private bool IsMorning(DateTime time) => time.Hour >= 5 && time.Hour < 12;
    private bool IsAfternoon(DateTime time) => time.Hour >= 12 && time.Hour < 18;

    private bool IsGreeting(string input)
    {
        string[] words = input.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (IsSimilarToGreetings(word))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsSimilarToGreetings(string word)
    {
        string[] greetings = morningGreetings.Concat(afternoonGreetings)
                                        .Concat(eveningGreetings)
                                        .Concat(genericGreetings)
                                        .ToArray();

        foreach (string greeting in greetings)
        {
            if (ComputeLevenshteinDistance(word, greeting) <= ComputeThreshold(greeting))
            {
                return true;
            }
        }
        return false;

    }

    private int ComputeLevenshteinDistance(string word1, string word2)
    {
        int[,] distances = new int[word1.Length + 1, word2.Length + 1];

        for (int i = 0; i <= word1.Length; i++)
        {
            distances[i, 0] = i;
        }

        for (int j = 0; j <= word2.Length; j++)
        {
            distances[0, j] = j;
        }

        for (int i = 1; i <= word1.Length; i++)
        {
            for (int j = 1; j <= word2.Length; j++)
            {
                int cost = (word1[i - 1] == word2[j - 1]) ? 0 : 1;

                distances[i, j] = Math.Min(
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost);
            }
        }

        return distances[word1.Length, word2.Length];
    }

    private double ComputeThreshold(string greeting)
    {
        return (greeting.Length <= 5) ? 1.0 : 0.5;
    }

    private string GetRandomResponse(string[] responses, DateTime now)
    {
        Random random = new Random();
        int index = random.Next(responses.Length);
        return responses[index];
    }
    #endregion

    #region TechnicalSupport
    private object HandleTechnicalSupport(string option)
    {
        switch (option)
        {
            case "Application Errors":
                return new { message = "If you're encountering application errors, please check the error message for details. You can also try clearing your browser cache or restarting your device. If the issue persists, contact support for assistance.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            case "Server Connectivity":
                return new { message = "If you're experiencing server connectivity issues, make sure you have a stable internet connection. You can also check if the server is undergoing maintenance or experiencing downtime. If the problem persists, contact your system administrator for assistance.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            case "Database Access":
                return new { message = "If you're unable to access the database, verify that your credentials are correct and that the database server is running. Check for any firewall or network restrictions that may be blocking access. If you still cannot access the database, contact your database administrator for further assistance.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            default:
                return new { message = "I'm sorry, I'm not sure how to help with that at the moment.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
        }
    }
    #endregion

    private object HandleProblemSolution(string option)
    {
        switch (option)
        {
            case "Fever":
                return GenerateProblemSolutionResponse(option, "General Practitioner");
            case "Headache":
                return GenerateProblemSolutionResponse(option, "General Practitioner");
            case "Stomach ache":
                return GenerateProblemSolutionResponse(option, "General Practitioner");
            case "Sprain":
                return GenerateProblemSolutionResponse(option, "Orthopedic Surgeon");
            case "Cut":
                return GenerateProblemSolutionResponse(option, "General Practitioner");
            case "Burn":
                return GenerateProblemSolutionResponse(option, "Emergency Services");
            case "Diabetes":
                return GenerateProblemSolutionResponse(option, "Specialist");
            case "Asthma":
                return GenerateProblemSolutionResponse(option, "Specialist");
            case "Hypertension":
                return GenerateProblemSolutionResponse(option, "General Practitioner");
            default:
                return new { message = "I'm sorry, I'm not sure how to help with that at the moment.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
        }
    }

    private async Task<object> GenerateProblemSolutionResponse(string problem, string specialist)
    {
        try
        {
            var allDoctors = await GetDoctors(specialist);
            var random = new Random();
            var doctors = allDoctors.OrderBy(x => random.Next()).Take(5);

            if (doctors.Any())
            {
                return new
                {
                    message = $"For {problem}, consult one of the following {specialist}s:",
                    date = DateTime.Now.ToShortDateString(),
                    time = DateTime.Now.ToShortTimeString(),
                    doctors = doctors,
                    problemSolutionInformation = InfoForPorblem(problem)
                };
            }
            else
            {
                return new                {
                    message = $"I'm sorry, there are currently no {specialist}s available to handle {problem}.",
                    date = DateTime.Now.ToShortDateString(),
                    time = DateTime.Now.ToShortTimeString()
                };
            }
        }
        catch (Exception ex)
        {
            // Log the exception and return an error response
            Console.WriteLine($"Error in GenerateProblemSolutionResponse: {ex.Message}");
            return new
            {
                message = "An error occurred while processing the request.",
                date = DateTime.Now.ToShortDateString(),
                time = DateTime.Now.ToShortTimeString()
            };
        }
    }
    private async Task<IEnumerable<object>> GetDoctors(string specialist)
    {
        try
        {
            if (specialist == null || specialist == string.Empty)
                return new List<object>();


            var alldoctors = _userManager.Users.AsEnumerable().Where(x => x.Role == "Doctor").ToList();
            var listOfSpecialist = new List<object>();
            foreach (var item in alldoctors)
            {
                var doc = await _doctorService.GetDoctor(item.Id);
              
                if (doc.Specialisation.Contains(specialist))
                {
                    listOfSpecialist.Add(new
                    {
                        doctorId = item.Id,
                        doctorName = item.Name,
                        phone = item.PhoneNumber,
                        email = item.Email,
                        specialization = doc.Specialisation,
                        location = item.Address,
                    });
                }
                 
            }


            return listOfSpecialist;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GenerateProblemSolutionResponse: {ex.Message}");
            return new[]
            {
                new
                {
                     message = "An error occurred while processing the request.",
                    date = DateTime.Now.ToShortDateString(),
                    time = DateTime.Now.ToShortTimeString()
                }
            };
        }
       
    }



    private string InfoForPorblem(string option)
    {
        switch (option)
        {
            case "Fever":
                return "For fever, get plenty of rest, drink fluids, and take fever-reducing medication such as acetaminophen.";
            case "Headache":
                return "For headache, try over-the-counter pain relievers and ensure you are well-hydrated. If symptoms persist, consult a healthcare professional." ;
            case "Stomach ache":
                return "For stomach ache, avoid heavy meals, drink ginger tea, and consider over-the-counter antacids. If pain persists or worsens, seek medical attention.";
            case "Sprain":
                return "For sprains, remember RICE: Rest, Ice, Compression, Elevation. If severe pain or swelling occurs, consult a healthcare provider.";
            case "Cut":
                return "For minor cuts, clean the wound with soap and water, apply an antibiotic ointment, and cover with a bandage. Seek medical attention for deep or large cuts.";
            case "Burn":
                return "For minor burns, run cool water over the burn for 10-15 minutes and apply aloe vera gel. Seek medical attention for severe burns or if the burn covers a large area."; 
            case "Diabetes":
                return "For diabetes management, monitor blood sugar levels regularly, eat a balanced diet, exercise regularly, and take medication as prescribed by your doctor.";
            case "Asthma":
                return "For asthma, use inhalers as prescribed by your doctor, avoid triggers such as smoke and allergens, and seek medical attention if symptoms worsen.";
            case "Hypertension":
                return "For hypertension, maintain a healthy lifestyle with regular exercise, a balanced diet low in sodium, and medication as prescribed by your doctor. Monitor blood pressure regularly.";
            // Add more cases for other options...
            default:
                return "I'm sorry, I'm not sure how to help with that at the moment.";
        }
    }

    private string DoctorSpecialistInfo(string option)
    {
        switch (option)
        {
            case "General Practitioner":
                return "General practitioners provide primary care services for a wide range of health issues. They can diagnose and treat common illnesses and injuries.";
            case "Cardiologist":
                return "Cardiologists specialize in diagnosing and treating diseases and conditions related to the heart and blood vessels.";
            case "Dermatologist":
                return "Dermatologists specialize in diagnosing and treating conditions related to the skin, hair, and nails.";
            case "Orthopedic Surgeon":
                return "Orthopedic surgeons specialize in diagnosing and treating injuries and conditions affecting the musculoskeletal system, including bones, joints, ligaments, tendons, and muscles.";
            case "Neurologist":
                return "Neurologists specialize in diagnosing and treating disorders of the nervous system, including the brain, spinal cord, and nerves.";
            case "Ophthalmologist":
                return "Ophthalmologists specialize in diagnosing and treating diseases and conditions related to the eyes and visual system.";
            case "Pediatrician":
                return "Pediatricians specialize in providing medical care for infants, children, and adolescents.";
            case "Psychiatrist":
                return "Psychiatrists specialize in diagnosing and treating mental illnesses and emotional disorders.";
            case "Psychologist":
                return "Psychologists specialize in assessing and treating mental health issues using psychotherapy and other interventions.";
            case "Licensed therapist":
                return "Licensed therapists provide counseling and therapy to individuals, couples, and families to address mental health concerns and improve well-being.";
            default:
                return "I'm sorry, I'm not sure how to help with that at the moment.";
        }
    }

    private async Task<object> HandleFindDoctor(string option)
    {
        try
        {
            var allDoctors = await GetDoctors(option);
            var random = new Random();
            var doctors = allDoctors.OrderBy(x => random.Next()).Take(5);

            if (doctors.Any())
            {
                return new
                {
                    date = DateTime.Now.ToShortDateString(),
                    time = DateTime.Now.ToShortTimeString(),
                    doctors = doctors,
                    problemSolutionInformation = DoctorSpecialistInfo(option)
                };
            }
            else
            {
                return new
                {
                    message = $"I'm sorry, there are currently no {option}s available in the system.",
                    date = DateTime.Now.ToShortDateString(),
                    time = DateTime.Now.ToShortTimeString()
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in HandleFindDoctor: {ex.Message}");
            return new
            {
                message = "An error occurred while processing the request.",
                date = DateTime.Now.ToShortDateString(),
                time = DateTime.Now.ToShortTimeString()
            };
        }
    }

    private object HandleMedicalAssistance(string option)
    {
        switch (option)
        {
            case "Emergency Services":
                // You can provide information about emergency services here
                return new { message = "In case of emergencies, dial 911 or go to the nearest emergency room.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            case "Medical Advice Hotline":
                // You can provide information about medical advice hotline here
                return new { message = "For medical advice, call your local medical advice hotline. They can provide guidance on non-emergency medical concerns.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            case "Pharmacy Information":
                // You can provide information about pharmacies here
                return new { message = "For pharmacy information, visit your local pharmacy or call them for assistance with prescriptions and over-the-counter medications.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
            // Handle other options similarly...
            default:
                return new { message = "I'm sorry, I'm not sure how to help with that at the moment.", date = DateTime.Now.ToShortDateString(), time = DateTime.Now.ToShortTimeString() };
        }
    }
}
