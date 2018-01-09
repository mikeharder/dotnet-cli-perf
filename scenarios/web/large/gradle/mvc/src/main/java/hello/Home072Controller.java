package hello;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class Home072Controller {

    @RequestMapping("/home072")
    public String index(Model model) {
        model.addAttribute("title", "InitialValue");
        return "index072";
    }

}
