package hello;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class Home039Controller {

    @RequestMapping("/home039")
    public String index(Model model) {
        model.addAttribute("title", "InitialValue");
        return "index039";
    }

}
