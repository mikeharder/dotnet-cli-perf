package hello;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ApplicationContext;
import org.springframework.http.ResponseEntity;
import org.springframework.util.StopWatch;
import org.springframework.web.client.RestTemplate;

@SpringBootApplication
public class Application {

    public static void main(String[] args) {
    	
    		StopWatch stopwatch = new StopWatch("dotnet-cli-perf");
    		
    		stopwatch.start("App Startup");
        ApplicationContext context = SpringApplication.run(Application.class, args);
        stopwatch.stop();
        
        stopwatch.start("First Request");
		RestTemplate template = new RestTemplate();
		ResponseEntity<String> response = template.getForEntity("http://localhost:8080", String.class);
		if (response.getStatusCodeValue() != 200) {
			throw new RuntimeException("Response was: " + response.getStatusCodeValue());
		}
		stopwatch.stop();

		System.out.println();
		System.out.println("Total time to request: " + stopwatch.getTotalTimeMillis() + "ms");
		System.out.println();
		System.out.println();
		System.out.print(stopwatch.prettyPrint());
		System.out.println();
		
		System.exit(0);
    }
}
